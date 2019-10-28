import { User } from "./user.model";
import { RolesEnum } from "./roles.enum";

export class Candidate {

  constructor(
    public id: string,

    //TODO: userProfile :
    //public Photo: Image,
    //public gender: GenderEnum = GenderEnum.female,
    //public address: Address,
    //public createdDate: Date,
    //public updatedDate: Date,
    //public BirthDate : Date,

    public firstName: string = "",
    public lastName: string = "",
    public email: string = "",
    public jobTitle: string = "SDE", //TODO: predefined jobs titles

    public status: string = RolesEnum.TW, //TODO: can be TW| PLU | Contractor ?
    public invited: boolean = false,
    public birthPlace: string = "",
    public nationality: string = "",
    public ssnumber: string = "", //regexpr
    public user: User = null//CandidateUser 
    //public IList<string> ConsultantsIDs { get; set; } = new List<string>();   /// List of the consultants ids, the candidate belong to
  ) { }


  get fullName() {
    return this.firstName + ' ' + this.lastName;
  }
}
