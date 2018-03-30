import {DetailPage} from "./DetailPage";

export interface PageResult{
    pageNumber:number;
    pageSize: number;
    total: number;
    searchWord:string;
    data: DetailPage[];
}