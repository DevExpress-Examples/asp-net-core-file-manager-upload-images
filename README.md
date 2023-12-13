<!-- default badges list -->
![](https://img.shields.io/endpoint?url=https://codecentral.devexpress.com/api/v1/VersionRange/230396054/19.2.3%2B)
[![](https://img.shields.io/badge/Open_in_DevExpress_Support_Center-FF7200?style=flat-square&logo=DevExpress&logoColor=white)](https://supportcenter.devexpress.com/ticket/details/T848532)
[![](https://img.shields.io/badge/📖_How_to_use_DevExpress_Examples-e9f6fc?style=flat-square)](https://docs.devexpress.com/GeneralInformation/403183)
<!-- default badges end -->
# File Uploader - How to upload images in Razor Pages

This example shows how to use the [DevExtreme FileUploader](https://js.devexpress.com/Documentation/Guide/UI_Components/FileUploader/) to upload images and display links for these images in an ASP.NET Core Razor Pages application. 


## Files to look at:

* [Index.cshtml](./CS/FileUploadRazorPages/Pages/Index.cshtml)
* [Index.cshtml.cs](./CS/FileUploadRazorPages/Pages/Index.cshtml.cs)
* [_Layout.cshtml](./CS/FileUploadRazorPages/Pages/_Layout.cshtml)

## Implementation Details

> **Note** The project targets .NET Core 3.1. To run the project in Visual Studio 2017, change the target framework in the project settings.

1) [Configure a Visual Studio Project](https://docs.devexpress.com/AspNetCore/401026/devextreme-based-controls/get-started/configure-a-visual-studio-project).

2) Add the [FileUploader](https://docs.devexpress.com/AspNetCore/DevExtreme.AspNet.Mvc.Builders.FileUploaderBuilder) component to your Razor Page. Specify its [Name](https://docs.devexpress.com/AspNetCore/DevExtreme.AspNet.Mvc.Builders.FileUploaderBuilder.Name.overloads) and [UploadUrl](https://docs.devexpress.com/AspNetCore/DevExtreme.AspNet.Mvc.Builders.FileUploaderBuilder.UploadUrl.overloads) properties.

    ```cs
    @(Html.DevExtreme().FileUploader()
        .Name("uploadedFile")
        .ChunkSize(200000)
        .UploadUrl(Url.Page("Index", "FileUpload"))
    ```

3) Use the 'POST' handler method and implement the [Chunk Upload](https://js.devexpress.com/jQuery/Documentation/Guide/UI_Components/FileUploader/Upload_Files/Server-Side_Implementation_in_ASP.NET/#Chunk_Upload) technique.  For the chunk or Ajax upload, make sure that **the first parameter** in your handler equals the **FileUploader.Name** parameter value.

4) Add the token value to the file uploader's requests because ASP.NET Core Razor Pages validate the [Antiforgery Token](https://docs.microsoft.com/en-us/aspnet/core/security/anti-request-forgery) in POST requests. 

    ```js
    (function () {
            var send = XMLHttpRequest.prototype.send;
            XMLHttpRequest.prototype.send = function (data) {
                data.append('__RequestVerificationToken', $('[name=__RequestVerificationToken]').val());
                return send.apply(this, arguments);
            };
        }());
    ```

5) (*Optional*) To obtain an uploaded image's URL, modify the 'ProcessUploadedFile' method so that it returns a relative path to your file. Return this URL from your handler method.

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

5) (*Optional*) Handle the [FileUploader.OnUploaded](https://docs.devexpress.com/AspNetCore/DevExtreme.AspNet.Mvc.Builders.FileUploaderBuilder.OnUploaded.overloads) event and get an image URL from the *args.request.response* property. 

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

## Documentation

[Upload Files: Server-Side Implementation in ASP.NET](https://js.devexpress.com/Documentation/Guide/UI_Components/FileUploader/Upload_Files/Server-Side_Implementation_in_ASP.NET/)

## More Examples

* [Angular File Uploader - How to upload images using ASP.NET Core API Controller](https://github.com/DevExpress-Examples/Angular-File-Uploader-How-to-upload-images-using-ASP.NET-Core-API-Controller)

* [DataGrid - How to use FileUploader in an edit form](https://github.com/DevExpress-Examples/DataGrid---How-to-use-FileUploader-in-an-edit-form)
