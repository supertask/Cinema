Cinema
====

**Cinema** is a collection of custom post-processing effects for Unity's [High Definition Render Pipeline](https://docs.unity3d.com/Packages/com.unity.render-pipelines.high-definition@11.0/manual/index.html) (HDRP) such as Keijiro's Kino.

Cinema is **HDRP version of kaiware007's shader** which is on the repo, [UnityVJShaderSlide20181108](https://github.com/kaiware007/UnityVJShaderSlide20181108).

[Cinema](https://user-images.githubusercontent.com/5733604/140326349-455c261a-892b-4f3d-86dd-d7490851d47f.mov)


[HDRP]:
    https://docs.unity3d.com/Packages/com.unity.render-pipelines.high-definition@latest

System Requirements
-------------------

- Unity 2021.1
- HDRP 11.0

Effects
-------

### Mosaic

![screenshot](./images/mosaic.gif)

**Mosaic** is .. 

### Radiation Blur

![screenshot](./images/radiationBlur.gif)


### Distortion

### Noise distortion
![screenshot](./images/noiseDistortion.gif)

### Barrel distortion
![screenshot](./images/barrelDistortion.gif)

### RGB Shift

![screenshot](./images/rgbShift.gif)


### Random invert

![screenshot](./images/randomInvert.jpg)

### Rect Block Glitch

### Reflection


## How to install


Write a package path on manifest.json

```
"jp.supertask.cinema.post-processing": "https://github.com/supertask/Cinema.git?path=/Packages/jp.supertask.cinema.post-processing",
```


## How to setup

Define custom post processing orders to make them take effect in the pipeline.

![screenshot](./images/postProcessingSetup.jpg)


## More effects?

Use [Kino](https://github.com/keijiro/Kino) for edge, streak, and more image effects.

I am planing to add more cinematic image effects in the future.. Check out the list [New Filters](https://github.com/supertask/Cinema/issues/2). And let me know incredible image effects you know.


## Key binders

- F1 key: Mosaic
- F2 key: Radiation Blur
- F3 key: Rect Block Glitch
- F4 key: Noise Distortion
- F5 key: Barrel Distortion
- F6 key: RGB Shift
- F7 key: Random Invert
- F8 key: Reflection (bottom to top)
- F9 key: Reflection (right to left)

## Acknowledge

- kaiware007, UnityVJShaderSlide20181108, https://github.com/kaiware007/UnityVJShaderSlide20181108
	- He is one of the super coders I know.
- XJINE, Unity_ImageFilters, https://github.com/XJINE/Unity_ImageFilters 
- Ian McEwan, Ashima Arts (Simplex noise)
- Stefan Gustavson (Classic noise and others)
