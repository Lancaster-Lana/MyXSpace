
export class Tenant
{
  public id: string;// GUID
  public name: string; //= BrandName
  public host: string;
  public theme: string;
  public connectionString: string;
  public isActive: boolean;

  constructor(name?: string)
  {
      this.name = name;
  }

}
