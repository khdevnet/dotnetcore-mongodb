using System;
using System.Diagnostics;
using System.Threading;
using Docker.DotNet;
using MongoDB.Bson.Serialization;
using MongoDB.Migrations.Common.DockerHttpClient;

namespace MongoDB.Migrations.Common
{
    public class MongoDockerFixture : IDisposable
    {
        private const string MongoDbImageName = "mongo";
        private const string MongoDbImageTag = "4.0";
        private const string MongoDbImage = MongoDbImageName + ":" + MongoDbImageTag;
        private const string MongoDbContainerName = "mongo-tests";
        private const string ExposedPort = "27017/tcp";
        private readonly DockerClient docker;
        public readonly string HostPort = "33381";

        public MongoDockerFixture()
        {
            docker = new DockerClientConfiguration(new Uri("npipe://./pipe/docker_engine")).CreateClient();

            docker.CreateImageIfNotExist(MongoDbImageName, MongoDbImageTag).Wait();
            docker.RemoveContainerIfExist(MongoDbContainerName).Wait();

            // docker run --name mongo-tests -p 33381:27017 -d mongo:4;
            var containerId = docker.RunContainer(MongoDbImage, MongoDbContainerName, ExposedPort, HostPort).Result;

            docker.WaitBeforeContainerInit(containerId).Wait();
        }

        public void Dispose()
        {
            docker.RemoveContainerIfExist(MongoDbContainerName).Wait();
            docker.Dispose();
        }
    }
}
