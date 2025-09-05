# OpenWaifu

# Abandoned for now. May be will finish some day.

Progress (Top - newest)

[https://youtu.be/wlIBgkIWuyI](https://youtu.be/wlIBgkIWuyI)  

[https://youtu.be/Nu91NxKmh-k](https://youtu.be/Nu91NxKmh-k)  

[https://youtu.be/KBZrHFgAmYs?si=0AQRcruAmrM9nIyf](https://youtu.be/KBZrHFgAmYs)  

CURRENTLY REQUIRES an OPEN AI KEY (Whisper, GPT)  
DON'T SHARE YOUR BUILD WITH ANYONE, BECAUSE IT CONTAINS YOUR API KEY!  

Unity 2022.3.5f1

Install conda  
https://conda.io

Create a new environment  
conda create -n waifu python=3.9.17  
conda activate waifu  

Install dependencies  
conda install -c nvidia cudatoolkit OR conda install -c conda-forge cudatoolkit  
conda install pytorch pytorch-cuda=11.7 -c pytorch -c nvidia  
//pip3 install torch torchvision torchaudio --index-url https://download.pytorch.org/whl/cu117 //   
pip install -r requirements.txt

# VoiceVox 

## Windows
Install CUDA ToolKit  
https://developer.nvidia.com/cuda-11-7-0-download-archive?target_os=Windows&target_arch=x86_64&target_version=10&target_type=exe_local

### CPU
docker pull voicevox/voicevox_engine:cpu-latest  
docker run --rm -it -p '127.0.0.1:50021:50021' voicevox/voicevox_engine:cpu-latest  

### GPU
docker pull voicevox/voicevox_engine:nvidia-latest  
docker run --rm --gpus all -p '127.0.0.1:50021:50021' voicevox/voicevox_engine:nvidia-latest

## Ubuntu Linux  
Install Cuda Toolkit  
sudo apt install nvidia-cuda-toolkit  
https://docs.nvidia.com/datacenter/cloud-native/container-toolkit/latest/distro/ubuntu.html?highlight=ubuntu  

### CPU
docker pull voicevox/voicevox_engine:cpu-latest  
docker run --rm -it -p '127.0.0.1:50021:50021' voicevox/voicevox_engine:cpu-latest  

### GPU
docker pull voicevox/voicevox_engine:nvidia-latest  
docker run --rm --gpus all -p '127.0.0.1:50021:50021' voicevox/voicevox_engine:nvidia-latest


# Characters  
https://www.chub.ai/characters  
https://booru.plus/+pygmalion  

Create a json file for pygmalion:  
https://zoltanai.github.io/character-editor/

# Run
Go to the Python directory and run main.py
