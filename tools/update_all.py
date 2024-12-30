#!/usr/bin/python
import subprocess

class Colors:
    HEADER = '\033[93m\033[1m'
    END = '\033[0m'

clients = [
    "../src/MawWww",
    "../src/solid-binary-clock",
    "../src/solid-google-maps",
    "../src/solid-learning",
    "../src/solid-memory",
    "../src/solid-money-spin",
    "../src/solid-weekend-countdown",
    "../src/webgl-blender-model",
    "../src/webgl-cube",
    "../src/webgl-shader",
    "../src/webgl-text"
]

# update client dependencies
for client in clients:
    print(f"{Colors.HEADER}Updating: {client}{Colors.END}")

    subprocess.run(
        ["bun", "install"],
        cwd = client
    )

    subprocess.run(
        ["bun", "update"],
        cwd = client
    )

    subprocess.run(
        ["bun", "run", "build"],
        cwd = client
    )

# update .net libs/apps
subprocess.run(["dotnet", "outdated", "-u"], cwd = "..")
subprocess.run(["dotnet", "build"], cwd = "..")
