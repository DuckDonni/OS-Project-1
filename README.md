Project Overview

This project is divided into two sections:
    Project A (Multi-Threading): A banking system simulation using multiple threads, demonstrating synchronization and deadlock resolution.

    Project B (Inter-Process Communication - IPC): A producer-consumer system using named pipes (FIFO) for process communication.

Environment Setup
    OS: Ubuntu
    Development Tools:
        - .NET SDK 9
        - C# Compiler
        - Linux terminal utilities for (mkfifo named pipes)

    Installation Steps:
        sudo apt update
        sudo apt install -y dotnet-sdk-9.0

    Running the Project
        Project A
            Move into the project
            cd project-a

            run the project
            dotnet run

        Project B
            Move into the  project
            cd project-b

            Open two terminals and run
            dotnet run --project consumer
            dotnet run --project producer