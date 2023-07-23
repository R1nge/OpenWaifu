# Waifu

Install cuda
https://developer.nvidia.com/cuda-downloads

Install conda
https://conda.io

Create new environment
conda create -n waifu python=3.9.17
conda activate waifu

pip install pytorch pytourch-cuda=11.7 -c pytorch -c nvidia  
pip install transformers  
pip install chardet  
pip install accelerate   
