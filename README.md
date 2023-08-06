# Waifu

Install conda  
https://conda.io

Create new environment  
conda create -n waifu python=3.9.17  
conda activate waifu  

Install dependencies  
conda install pytorch pytorch-cuda=11.7 -c pytorch -c nvidia  
pip install transformers  
pip install accelerate   
pip install pyzmq  
pip install pysentimiento

# VoiceVox 

## Windows

### CPU
docker pull voicevox/voicevox_engine:cpu-ubuntu20.04-latest  
docker run --rm -p 0.0.0.0:50021:50021 --env VV_CPU_NUM_THREADS=8 voicevox/voicevox_engine:cpu-ubuntu20.04-latest  

### GPU
docker pull voicevox/voicevox_engine:nvidia-ubuntu20.04-latest  
docker run --rm --gpus all -p 0.0.0.0:50021:50021 --env VV_CPU_NUM_THREADS=8 voicevox/voicevox_engine:nvidia-ubuntu20.04-latest  


## Linux  
Install Cuda Toolkit  
sudo apt install nvidia-cuda-toolkit  
https://docs.nvidia.com/datacenter/cloud-native/container-toolkit/latest/distro/ubuntu.html?highlight=ubuntu  

### CPU
docker pull voicevox/voicevox_engine:cpu-ubuntu20.04-latest  
docker run --rm -p 0.0.0.0:50021:50021 --env VV_CPU_NUM_THREADS=8 voicevox/voicevox_engine:cpu-ubuntu20.04-latest  

### GPU
docker pull voicevox/voicevox_engine:nvidia-ubuntu20.04-latest  
sudo docker run --rm --runtime=nvidia --gpus all -p 0.0.0.0:50021:50021 --env VV_CPU_NUM_THREADS=8 voicevox/voicevox_engine:nvidia-ubuntu20.04-latest


# Characters  

https://www.chub.ai/characters  
https://booru.plus/+pygmalion  

Create json:  
https://zoltanai.github.io/character-editor/