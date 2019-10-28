
export class Client {

  constructor(
    public id: string,
    public clientName: string = "",
    public clientCode: string = "",
    public invited: boolean = false,
    public status: string = "", //todo: business
    public email: string = "",
    public phoneNumber: string = "" //regexpr,

    //TODO: userProfile or client user data
    //public gender: boolean = false,//male =0 , female = 1;
    //public address: Address,
    //public createdDate: Date, public updatedDate: Date,
  ){ }
}
