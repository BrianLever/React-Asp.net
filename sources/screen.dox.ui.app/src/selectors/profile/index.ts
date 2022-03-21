import { IRootState } from '../../states';

export const isProfileLoadingSelector = (state: IRootState) => state.profile.isProfileLoading;
export const isProfileErrorSelector = (state: IRootState) => state.profile.isProfileError;
export const getProfileSelector = (state: IRootState) => state.profile.profile;
export const getFullNameSelector = (state: IRootState) => state.profile.profile ? state.profile.profile.FullName : '';
export const getFirstNameSelector = (state: IRootState) => state.profile.profile ? state.profile.profile.FirstName : '';