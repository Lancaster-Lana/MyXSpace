//There are STATIC permissions. Each role may have several permission (permissions groups) 
export type PermissionNames =

  "View Users" | "Manage Users" |
  "View Roles" | "Manage Roles" | "Assign Roles" |

  "View Contracts" | "Manage Contracts" | "Sign Contracts" |
  "View Candiates" | "Manage Candidates" | "Invite Candidates" |
  "View Clients" | "Manage Clients" |"Invite Clients";

export type PermissionValues =

   //Admin group permissions
    "users.view" | "users.manage" |
    "roles.view" | "roles.manage" | "roles.assign" |

   //Contract permissions (can be shared):  some permissions may belong to several Roles (for example, sign contract by Consultant, by Candidate, by Client )
   "contracts.view" | "contracts.manage" | "contracts.sign" |
   "candidates.view" | "candidates.manage" | "candidates.invite" |
   "clients.view" |"clients.manage"|"clients.invite" 

export class Permission {

  public static readonly viewUsersPermission: PermissionValues = "users.view";
  public static readonly manageUsersPermission: PermissionValues = "users.manage";

  public static readonly viewRolesPermission: PermissionValues = "roles.view";
  public static readonly manageRolesPermission: PermissionValues = "roles.manage";
  public static readonly assignRolesPermission: PermissionValues = "roles.assign";

  public static readonly ViewContracts = "contracts.view";
  public static readonly ManageContracts = "contracts.manage";
  public static readonly SignContracts = "contracts.sign";

  public static readonly ViewCandidates = "candidates.view";
  public static readonly ManageCandidates = "candidates.manage";
  public static readonly InviteCandidates = "candidates.invite"; //to join tenant\brand

  public static readonly ViewClients = "clients.view";
  public static readonly ManageClients = "clients.manage";
  public static readonly InviteClients = "clients.invite"; //to join tenant\brand


  public name: PermissionNames;
  public value: PermissionValues;
  public groupName: string;
  public description: string;

  constructor(name?: PermissionNames, value?: PermissionValues, groupName?: string, description?: string) {
    this.name = name;
    this.value = value;
    this.groupName = groupName;
    this.description = description;
  }
}
