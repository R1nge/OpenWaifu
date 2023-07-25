# Waifu

Install cuda
https://developer.nvidia.com/cuda-downloads

Install conda
https://conda.io

Create new environment  
conda create -n waifu python=3.9.17  
conda activate waifu  

Install dependencies  
conda install pytorch pytorch-cuda=11.8 -c pytorch -c nvidia  
conda install -c "nvidia/label/cuda-12.2.0" cuda  
conda install -c conda-forge cudatoolkit  
pip install transformers  
pip install chardet  
pip install accelerate   
pip install pyzmq  
pip install bitsandbytes

NLLP 2.9GB VRAM
