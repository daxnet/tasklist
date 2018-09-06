export class TaskItem {
    id: string;
    name: string;
    hasDone: boolean;
    dateOpened: Date;
    dateDone: Date;

    public get dateOpenedString(): string {
        // return this.dateOpened.toLocaleDateString();
        return 'sdf';
    }
}
