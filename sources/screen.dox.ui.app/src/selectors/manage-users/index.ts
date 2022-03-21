import { IRootState } from '../../states';

export const getManageUserListSelector = (state: IRootState) => state.manageUsers.users;
export const isManageUserListLoading = (state: IRootState) => state.manageUsers.isUsersLoading;
export const manageUsersCurrentPageSelector = (state: IRootState) => state.manageUsers.currentPage;
export const manageUsersTotalSelector = (state: IRootState) => state.manageUsers.totals;
export const manageUsersOrderKeySelector = (state: IRootState) => state.manageUsers.orderKey;
export const manageUsersOrderDirectionSelector = (state: IRootState) => state.manageUsers.orderDirection;
export const manageUsersLocationsSelector = (state: IRootState) => state.manageUsers.locations;
export const manageUsersLocationIdSelector = (state: IRootState) => state.manageUsers.selectedLocationId;
export const manageUsersUserSelector = (state: IRootState) => state.manageUsers.user;
export const manageUsersGroupsSelector = (state: IRootState) => state.manageUsers.userGroups;
export const manageUsersSelectedUserIdSelector = (state: IRootState) => state.manageUsers.selectedUserId;