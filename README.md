<!-- default badges list -->
![](https://img.shields.io/endpoint?url=https://codecentral.devexpress.com/api/v1/VersionRange/230396054/19.2.3%2B)
[![](https://img.shields.io/badge/Open_in_DevExpress_Support_Center-FF7200?style=flat-square&logo=DevExpress&logoColor=white)](https://supportcenter.devexpress.com/ticket/details/T848532)
[![](https://img.shields.io/badge/📖_How_to_use_DevExpress_Examples-e9f6fc?style=flat-square)](https://docs.devexpress.com/GeneralInformation/403183)
<!-- default badges end -->
# File Uploader - How to upload images in Razor Pages

This example shows how to upload images in an ASP.NET Core Razor Pages application using DevExtreme FileUploader. The project also shows how to display links to uploaded images.

*Files to look at*:

* [Index.cshtml](./CS/FileUploadRazorPages/Pages/Index.cshtml)
* [Index.cshtml.cs](./CS/FileUploadRazorPages/Pages/Index.cshtml.cs)
* [_Layout.cshtml](./CS/FileUploadRazorPages/Pages/_Layout.cshtml)


## Implementation:

1) Add required libraries to your project: [Configure a Visual Studio Project](https://docs.devexpress.com/AspNetCore/401026/devextreme-based-controls/get-started/configure-a-visual-studio-project).
> **Note** The project targets .NET Core 3.1. To run the project in Visual Studio 2017, change the target framework in the project settings.

2) Add the FileUploader control to your Razor Page. Specify the Name and UploadUrl properties.
```cs
@(Html.DevExtreme().FileUploader()
    .Name("uploadedFile")
    .ChunkSize(200000)
    .UploadUrl(Url.Page("Index", "FileUpload"))
```

3) Create the POST handler method and implement one of solutions from the [Server-Side Implementation in ASP.NET](https://js.devexpress.com/Documentation/Guide/Widgets/FileUploader/Upload_Files/Server-Side_Implementation_in_ASP.NET/) article. Here, FileUploader uses Chunk Upload, so we use the [Chunk Upload](https://js.devexpress.com/Documentation/Guide/Widgets/FileUploader/Upload_Files/Server-Side_Implementation_in_ASP.NET/#Chunk_Upload) approach.

> **Important** For Chunk or Ajax upload, make sure that **the first parameter** in your handler is the same as your **FileUploader.Name**

4) ASP.NET Core Razor Pages validate the [Antiforgery Token](https://www.learnrazorpages.com/security/request-verification) in POST requests. So, it's necessary to add the token value to the uploader's requests:
```js
(function () {
        var send = XMLHttpRequest.prototype.send;
        XMLHttpRequest.prototype.send = function (data) {
            data.append('__RequestVerificationToken', $('[name=__RequestVerificationToken]').val());
            return send.apply(this, arguments);
        };
    }());
```

5) (*Optional*) To obtain an uploaded image's URL, modify the "ProcessUploadedFile" method so that it returns a relative path to your file and then return this URL from your handler method:
```cs
 string ProcessUploadedFile(string tempFilePath, string fileName) {
            var path = Path.Combine(_hostingEnvironment.WebRootPath, "uploads", fileName);
            System.IO.File.Copy(tempFilePath, path);
            return Path.GetRelativePath(_hostingEnvironment.WebRootPath, path);
        }
public IActionResult OnPostFileUpload(IFormFile uploadedFile, string chunkMetadata) {
            ...
             return new JsonResult(new { imgUrl = resultImageUrl });
        }
```
5) (*Optional*) Handle the [FileUploader.OnUploaded](https://js.devexpress.com/Documentation/ApiReference/UI_Widgets/dxFileUploader/Configuration/#onUploaded) event and get an image URL from the *args.request.response* property. Append a link to this URl in the following manner:
```js
function OnUploaded(args) {
        let imgUrl = JSON.parse(args.request.response).imgUrl;
        let imgName = args.file.name;
        let imagesContainer = document.getElementById("imagesContainer");
        imagesContainer.appendChild(createLink(imgName, imgUrl));
    }
function createLink(imgName, url) {
        let link = document.createElement("A");
        link.innerText = imgName;
        link.href = url;
        return link;
    }
```


