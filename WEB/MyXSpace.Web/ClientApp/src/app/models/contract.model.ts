export class Contract {

  constructor(

    public id: string, // GUID
    public name: string = "", //TODO: in DB 

    public clientSigned: boolean = false,
    public clientSignOrigin: string = "",

    public internalUserSigned: boolean = false,

    public externalUserId: string = "",
    public externalUserSigned: boolean = false,
    public externalUserSignOrigin: string = "",

    public fullySigned: boolean = false, //todo: must be detected as clientSigned&internalUserSigned&externalUserSigned
  ) {}
}
