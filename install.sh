#!/bin/bash

if [[ $EUID -ne 0 ]]; then
   echo "This script must be run as root" 
   exit 1
fi

xbuild src/Blender.sln

cp src/Blender/bin/Debug/Blender.exe /usr/bin/blenderhash

echo "Copied Blender.exe to /usr/bin/blenderhash"
