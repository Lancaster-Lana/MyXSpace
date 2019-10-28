import { JobOfferStatus } from "./enums";

export class JobOffer {

  constructor(

    public id: string, // GUID
    public name: string = "", 
    public title: string = "", 
    public description: string = "", //TODO: in DB
    public status: JobOfferStatus = JobOfferStatus.New,
    public createdOn: Date,
    public expiredOn: Date,
  ) { }
}
