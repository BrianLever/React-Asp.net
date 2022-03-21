import { IRootState } from '../../states';
// import { IBranchLocationItemResponse } from '../../actions/age-groups';


export const getAgeGroupsValueSelector = (state: IRootState) => state.ageGroups.Value;
export const getAgeGroupsDefaultValueSelector = (state: IRootState) => state.ageGroups.DefaultValue;
export const IsAgeGroupsListLoading = (state: IRootState) => state.ageGroups.isAgeGroupLoading;
export const getAgeGroupsLablesSelector = (state: IRootState) => state.ageGroups.Labels; 