# Azure Cognitive Service Speech to Text

Before your need to create `Speech Service` via Azure Portal 
[Here](https://portal.azure.com/#create/Microsoft.CognitiveServicesSpeechServices) 

To get the key you can follow [this guideline](https://docs.microsoft.com/en-us/azure/cognitive-services/cognitive-services-apis-create-account?tabs=multiservice%2Cwindows#get-the-keys-for-your-resource)

Then config the key in `program.cs` in below section

``` C#
string YourSubscriptionKey = "<Your-Key>";
string YourServiceRegion = "<Region ex. eastasia>";
```

Start application by running `dotnet run` 