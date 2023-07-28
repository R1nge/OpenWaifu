# Waifu

Install conda
https://conda.io

Create new environment  
conda create -n waifu python=3.9.17  
conda activate waifu  

Install dependencies  
conda install pytorch torchvision torchaudio pytorch-cuda=11.7 -c pytorch -c nvidia  
pip install transformers  
pip install accelerate   
pip install pyzmq  
pip install pysentimiento

NLLB 2.9GB VRAM
