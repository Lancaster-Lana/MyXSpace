export class UserProfile {

 //public user: User;
  public fullName: string;
  public jobTitle: string;
  public photo: string;
  public address: string;

  constructor(
     fullName: string = "",
     jobTitle: string ="",
     photo: string = null,
     address: string  ="") {

    this.fullName = fullName;
    this.jobTitle = jobTitle;
    this.photo = photo;
    this.address = address;
  }

}
