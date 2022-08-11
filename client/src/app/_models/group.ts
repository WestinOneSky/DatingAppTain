export interface Group {
    name: string;
    connections: Connections[];
}

interface Connections{
    connectionsId: string;
    username: string;
}