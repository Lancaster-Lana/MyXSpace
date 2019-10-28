export class InviteViewModel {

  constructor(
    /// <summary>
    /// Tenant\Brand to join
    /// </summary>
    public TenantName: string,

    /// <summary>
    /// RoleName
    /// </summary>
    public Role: string,

    /// <summary>
    ///TODO: fromConsultant - email or name
    /// </summary>
    public InvitedBy: string,

    /// <summary>
    /// User email to be invited to tenant
    /// </summary>
    public ToEmail: string,

    /// <summary>
    /// User name to be invited to tenant
    /// </summary>
    public ToName: string

  ) { }
}
