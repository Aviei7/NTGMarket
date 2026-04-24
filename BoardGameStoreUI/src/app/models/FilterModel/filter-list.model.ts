import { QueryParam } from "./query-param.model";

export interface FilterList {
    id: number;
    filterName: string;
    fieldName: string;
    filterType: string;
    paramList: QueryParam[];
}
