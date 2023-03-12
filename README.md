# SFModelConverter
A program for converting models supported by SoulsFormats  
Currently only exports models with non-dynamic meshes, more support coming later  
The program currently has only been tested against Armored Core: For Answer FLVER0 models  
Armored Core: For Answer FLVER0 models can be exported to DAE currently but is somewhat incomplete  
Should support MDL4 models such as those from Armored Core 4, but the latest changes have not been tested  
Should support non-dynamic FLVER2 models, but this program has not been tested with FLVER2 at all yet.  
All other model types currently not supported or tested    

The "Convert" button is an option for testing that is currently outdated. Do not use it.  
Use the "Export" button for attempting exports.  

Flip and swap options are currently being tested and may not do what I think they are doing.  
"Flip" attempts to set the vectors mentioned in the options to negative.  
"Swap" attempts to literally swap the vectors mentioned in the options.  
Changing order of swaps in the case of a vector getting swapped more than once is currently not supported.  
The swap order will be the order the options are ordered in.  

I am currently working on this project and hope to fixed exports, and add import ability for SoulsFormats model types.

# Building
If you want to build the project you should clone it with these command in git bash:  
```
git clone https://github.com/WarpZephyr/SFModelConverter.git  
git clone https://github.com/WarpZephyr/SoulsFormats.git  
```
Then build it in visual studio

# Credits
Dropoff for helping with learning model transformations and so much more  
Pear for helping me with many model related things  
Nordgaren for great suggestions  
Huge thanks to the Dark Souls modding discord server  
Huge thanks to TK for making the SoulsFormats library
