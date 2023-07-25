# Waifu

Install cuda
https://developer.nvidia.com/cuda-downloads

Install conda
https://conda.io

Create new environment  
conda create -n waifu python=3.9.17  
conda activate waifu  

Install dependencies  
#conda install -c "nvidia/label/cuda-12.2.0" cuda  
#conda install -c conda-forge cudatoolkit  
conda install pytorch pytorch-cuda=11.7 -c pytorch -c nvidia  
pip install transformers  
pip install chardet  
pip install accelerate   
pip install pyzmq  
pip install bitsandbytes  
pip install scipy  

NLLB 2.9GB VRAM
