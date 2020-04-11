using Docker.DotNet;
using Docker.DotNet.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Xunit;
using Xunit.Abstractions;
using MongoDB.Migrations.Common.DockerHttpClient;

namespace DockerHttpClient.Tests
{
    public class DockerClientTests
    {
        [Fact]
        public async void CreateContainerTest()
        {
            var mongoDbImageName = "mongo";
            var mongoDbImageTag = "4.0";
            var mongodDbImage = $"{mongoDbImageName}:{mongoDbImageTag}";
            var mongoDbContainerName = "mongo-tests";
            var exposedPort = "27017/tcp";
            var hostPort = "33381";
            DockerClient docker = new DockerClientConfiguration(new Uri("npipe://./pipe/docker_engine"))
                 .CreateClient();

            await docker.CreateImageIfNotExist(mongoDbImageName, mongoDbImageTag);
            await docker.RemoveContainerIfExist(mongoDbContainerName);

            // docker run --name mongo-tests -p 33381:27017 -d mongo:4;
            var containerId = await docker.RunContainer(mongodDbImage, mongoDbContainerName, exposedPort, hostPort);

            await docker.WaitBeforeContainerInit(containerId);

            ContainerListResponse container = await docker.GetContainerByName(mongoDbContainerName);

            Assert.NotNull(container);

            await docker.RemoveContainerIfExist(mongoDbContainerName);

            docker.Dispose();
        }
    }
}
