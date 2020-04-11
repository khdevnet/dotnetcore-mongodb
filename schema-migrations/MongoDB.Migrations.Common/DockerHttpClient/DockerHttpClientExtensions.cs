using Docker.DotNet;
using Docker.DotNet.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MongoDB.Migrations.Common.DockerHttpClient
{
    public static class DockerHttpClientExtensions
    {
        public static async Task WaitBeforeContainerInit(this DockerClient client, string containerId)
        {
            Stream logsStream = await client.Containers.GetContainerLogsAsync(containerId, new ContainerLogsParameters() { ShowStdout = true, Follow = true });

            using (StreamReader sr = new StreamReader(logsStream))
            {
                string line;
                while ((line = sr.ReadLine()) != null && !line.Contains("waiting for connections on port 27017"))
                {
                    Console.WriteLine(line);
                }
            }
        }

        public static async Task<string> RunContainer(this DockerClient client, string image, string containerName, string exposedPort, string hostPort)
        {
            CreateContainerResponse container = await CreateContainer(client, image, containerName, exposedPort, hostPort);
            await client.Containers.StartContainerAsync(containerName, new ContainerStartParameters());
            return container.ID;
        }

        public static async Task<CreateContainerResponse> CreateContainer(this DockerClient client, string image, string containerName, string exposedPort, string hostPort)
        {
            return await client.Containers.CreateContainerAsync(new CreateContainerParameters()
            {
                Image = image,
                Name = containerName,
                ExposedPorts = new Dictionary<string, EmptyStruct>()
                {
                    {exposedPort, new EmptyStruct() }
                },
                HostConfig = new HostConfig
                {
                    PortBindings = new Dictionary<string, IList<PortBinding>>
                    {
                        {
                            exposedPort,
                            new List<PortBinding>
                            {
                                new PortBinding
                                {
                                    HostPort = hostPort
                                }
                            }
                        }
                    },
                }
            });
        }

        public static async Task CreateImageIfNotExist(this DockerClient client, string imageName, string tag)
        {
            //reference
            var filters = new Dictionary<string, IDictionary<string, bool>> {
                { "reference", new Dictionary<string, bool>{ { $"{imageName}:{tag}", true } } }
            };
            var images = await client.Images.ListImagesAsync(new ImagesListParameters { Filters = filters });
            if (images.Count == 0)
            {
                await client.Images.CreateImageAsync(
                    new ImagesCreateParameters { FromImage = imageName, Tag = tag }, null,
                    new Progress<JSONMessage>(message =>
                    {
                        Console.WriteLine(!string.IsNullOrEmpty(message.ErrorMessage)
                            ? message.ErrorMessage
                            : $"{message.ID} {message.Status} {message.ProgressMessage}");
                    }));
            }
        }

        public static async Task RemoveContainerIfExist(this DockerClient client, string name)
        {
            ContainerListResponse container = await GetContainerByName(client, name);

            if (container != null)
            {
                await client.Containers.RemoveContainerAsync(container.ID, new ContainerRemoveParameters() { Force = true });

            }
        }

        public static async Task<ContainerListResponse> GetContainerByName(this DockerClient client, string name)
        {
            var filters = new Dictionary<string, IDictionary<string, bool>>()
            {
                { "name", new Dictionary<string, bool>() { { name, true } } }
            };

            var containers = await client.Containers.ListContainersAsync(new ContainersListParameters()
            {
                All = true,
                Filters = filters
            });

            return containers.FirstOrDefault();
        }
    }
}
