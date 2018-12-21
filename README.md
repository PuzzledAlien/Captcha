# Captcha
项目中包含一个页面，运行之后，可访问路由“/Index”，进行自测，页面如下图所示

![avatar](https://github.com/PuzzledAlien/Captcha/blob/master/captcha.png?raw=true)

## 使用方式
嵌入图片，部署之后访问“/captcha”，可直接加载一张图片

接口:

  1、api/captcha ：返回图片byte[]

  2、api/captcha/verify：校验验证码

## 注意

如果运行在Windows环境，是完成ok的。[System.Drawing.Common](https://www.nuget.org/packages/System.Drawing.Common)是完美的解决方案。

如果现在你想要部署在Ubuntu或者Docker环境下，你需要安装 对应平台的 `GDI +`相关依赖项。

Ubuntu需要安装的依赖库如下

```
sudo apt install libc6-dev 
sudo apt install libgdiplus
```

## 下一步计划

### 计划1
.Net Core图形处理开源组件有更多选项，下一步计划都试一试

 - [ImageSharp](https://github.com/SixLabors/ImageSharp)
 - [Magick.NET](https://github.com/dlemstra/Magick.NET)
 - [SkiaSharp](https://github.com/mono/SkiaSharp)

### ~~计划2~~
~~尝试.Net Core 2.2，将Captcha独立成.Net Standard类库，并借助微软自带DI解耦CaptchaFactory~~

计划2已完成