import { UserDTO } from "@/api/generated";

export enum Step {
  Init = -1,
  Waiting,
  Confirmed,
}
export interface UserState {
  loading: boolean;
  users: UserDTO[] | null;
  user: UserDTO | null;
  step: Step;
  error: string | null;
  pageSize: number;
  pageNum: number;
  totalItems: number;
  totalPages: number;
}

export interface UploadAvatar {
  fileType: number;
  file: Blob;
}
