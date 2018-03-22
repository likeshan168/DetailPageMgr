
interface FileInfo {
    name: string;
    path: string;
}

export interface FileInfoResponse{
    fileList: FileInfo[];
    message: string;
    isSuccess: boolean;
}