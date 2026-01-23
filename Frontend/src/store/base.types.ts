export interface BaseState {
  loading: boolean;
  error: string | null;
}

export type FetchParams = {
  regions?: string[] | undefined;
  keyword?: string | undefined;
  pageSize?: string | undefined;
  pageNum?: string | undefined;
};

export enum PAGE_TYPE {
  MAX = "Max",
}

export enum STATUS_ACTION_TYPE {
  VIEW = "View",
  EDIT = "Edit",
}
