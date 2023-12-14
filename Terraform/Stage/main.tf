terraform {
  required_providers {
    docker = {
      source  = "kreuzwerker/docker"
      version = "~> 3.0.1"
    }
  }
}

provider "docker" {}

resource "docker_image" "postgres" {
  name         = "nginx:latest"
  keep_locally = false
}


resource "docker_container" "postgres" {
  image    = docker_image.postgres.image_id
  name     = "genia-postgres"
  restart  = "always"
  hostname = "genia-postgres"
  env      = ["host=localhost", "port=5432", "POSTGRES_USER=postgres", "POSTGRES_PASSWORD:123456"]
  volumes {
    container_path = "/docker-entrypoint-initdb.d"
    host_path      = "/volumes/postgres"
    read_only      = false
    consistency    = "consistent"
  }
  ports {
    internal = "5432"
    external = "5432"
  }
}

