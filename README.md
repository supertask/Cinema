Cinema
====

**Cinema** is a collection of custom post-processing effects for Unity's [High Definition Render Pipeline](https://docs.unity3d.com/Packages/com.unity.render-pipelines.high-definition@11.0/manual/index.html) (HDRP) such as Keijiro's Kino.

Cinema is **HDRP version of kaiware007's shader** which is on the repo, [UnityVJShaderSlide20181108](https://github.com/kaiware007/UnityVJShaderSlide20181108).

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

### Rect Block Glitch

![screenshot](./images/rectBlockGlitch.gif)

### Distortion

![screenshot](./images/distortion.gif)

### RGB Shift

![screenshot](./images/rgbShift.gif)


### Random invert

![screenshot](./images/randomInvert.jpg)

### Negative

![screenshot](./images/negative.jpg)

### Reflection

![screenshot](./images/reflection.jpg)


## How to install


Write a package path on manifest.json

```
"jp.supertask.shaderlibcore": "https://github.com/supertask/Cinema.git?path=/Packages/jp.supertask.cinema.post-processing",
```


## How to setup

Define custom post processing orders to make them take effect in the pipeline.

![screenshot](./images/postProcessingSetup.jpg)

## Acknowledge

- kaiware007, UnityVJShaderSlide20181108, https://github.com/kaiware007/UnityVJShaderSlide20181108
	- He is one of the super coders I know.
- Ian McEwan, Ashima Arts (Simplex noise)
- Stefan Gustavson (Classic noise and others)
