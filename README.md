# CyberPunkEffect

![image](https://user-images.githubusercontent.com/16706911/76066136-3c589380-5fa2-11ea-917e-ec85529c580c.png)




![Record_2020_03_06_12_07_50_569](https://user-images.githubusercontent.com/16706911/76066724-6199d180-5fa3-11ea-904e-0ac2b623ba2f.gif)
![Record_2020_03_06_12_08_48_299](https://user-images.githubusercontent.com/16706911/76066664-48912080-5fa3-11ea-9b0d-6bac6137143d.gif)


# 1. Cyberpunk 2077 scanning effect
This tutorial will describe step-by-step how to write a cyber punk like scanning effect in unity. The effect is made from different parts such as outline, pixelation and fill effect. You can see the effect in the following images:


![image](https://user-images.githubusercontent.com/16706911/76066897-b2a9c580-5fa3-11ea-9113-032b6d1b73a1.png)

![Record_2020_03_06_12_24_27_337](https://user-images.githubusercontent.com/16706911/76078657-57ce9900-5fb8-11ea-921f-b84640550de5.gif)

# 2. Objects mask
As you may know, we can implement outline effect and fill effect easily in regular object shaders using vertex distortion and screen space overlay effects, but the third effect which cannot be implemented using regular shaders is pixelation effect, this effect can only be created using image effects because it goes outside the object with this effect applied. The problem with this implementation is that we cannot apply the image effect on specific objects and it will be applied to the whole screen. So we need a way to mask our objects in our image effect shader.
There are two ways to implement object mask to use it in our effect:
1.Using unity’s pre-rendered stencil buffer
2.Using stencil buffer along with command buffers
3.Using command buffers


# 2.1 Using stencil buffer
A stencil buffer is an extra data buffer, in addition to color buffer and depth buffer. The buffer is per pixel and works on 8bit integer values.
In the simplest case, the stencil buffer is used to mask area of rendering. For example, you want to render a character everywhere but in a sphere.
As shown in the image below, before anything is drawn in stencil buffer, the buffer is filled with zeros for each pixel and is black and we can make it white by replacing the buffer values in the area of the object we want to mask with.

![image](https://user-images.githubusercontent.com/16706911/76067160-2fd53a80-5fa4-11ea-88ce-78208de39a22.png)
```
// Make my area of rendering’s stencil buffer white.
Stencil
{
    Ref 1
    Comp Always
    Pass Replace
}
```
Then create another default shader called “MaskedShader” and add the following lines as you did before, create a material of it and add it to the human.
```
// Draw me where stencil buffer is black
Stencil
{
    Ref 0
    Comp Equal
}
```
![image](https://user-images.githubusercontent.com/16706911/76067377-7e82d480-5fa4-11ea-95a4-7ff936d83289.png)



// old image effect technique and description
## Using stencil buffer along with command buffer

## Using only command buffers
// describing command buffers
## Image effect

## Outline effect
## Fill effect

![image](https://user-images.githubusercontent.com/16706911/76071779-07513e80-5fac-11ea-8b38-b1d49b4000a9.png)

## Pixelate effect
## Applying to multiple objects


# ToDo
Complete the Tutorial
