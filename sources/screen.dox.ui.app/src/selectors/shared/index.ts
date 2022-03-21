import { TBranchLocationsItemResponse, TScreeningProfileItemResponse } from '../../actions/shared';
import { IRootState } from '../../states';

export const getListBranchLocationsSelector = (s: IRootState): Array<TBranchLocationsItemResponse> =>  s.shared.branchLocationsArray;
export const getScreeningProfileListSelector = (s: IRootState): Array<TScreeningProfileItemResponse> =>  s.shared.screeningProfileList;
export const setCreateDateCustomOrGPRASelector = (s: IRootState): boolean => s.shared.isCreateDateCustom;