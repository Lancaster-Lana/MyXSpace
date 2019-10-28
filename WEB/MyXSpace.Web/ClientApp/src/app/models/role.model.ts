import { Permission } from './permission.model';

export class Role {

    constructor(name?: string, description?: string, permissions?: Permission[]) {

        this.name = name;
        this.description = description;
        this.permissions = permissions;
    }

    public roleId: string;
    public name: string;
    public description: string;
    public usersCount: string;
    public permissions: Permission[];
}
