export class AppSettings {

  public static get build(): string {
    return "local";
  }
  public static get ng2ENV(): string {
    return "Dev";
  }
  public static get ApiEndpoint(): string {
    return "http://informationcart.eastus2.cloudapp.azure.com:80/api/";
  }
  public static get ApiMaps(): string {
    return "http://informationcart.eastus2.cloudapp.azure.com:82/api/";
  }
  public static get ApiReports(): string {
    return "http://informationcart.eastus2.cloudapp.azure.com:84/api/";
  }
  public static get ApiAttraction(): string {
    return "http://informationcart.eastus2.cloudapp.azure.com:83/api/";
  }
}