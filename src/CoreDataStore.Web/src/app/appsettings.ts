export class AppSettings {

  public static get build(): string {
    return "local";
  }
  public static get buildId(): string {
    return "1.0.0";
  }
  public static get ng2ENV(): string {
    return null;
  }
  public static get ApiEndpoint(): string {
    return localStorage.getItem("ApiEndpoint") || "http://informationcart.eastus2.cloudapp.azure.com:80/api/";
  }
  public static get ApiMaps(): string {
    return localStorage.getItem("ApiMaps") || "http://informationcart.eastus2.cloudapp.azure.com:82/api/";
  }
  public static get ApiReports(): string {
    return "/api/reports";
  }
  public static get ApiAttraction(): string {
    return "/api/attraction";
  }
}
