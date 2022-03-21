import { IRootState } from '../../states';

export const getEhrLoginListSelector = (state: IRootState) => state.ehrLogin.ehrLoginList;
export const isEhrLoginListLoadingSelector = (state: IRootState) => state.ehrLogin.isLoading;
export const getEhrLoginAccessCodeSelector = (state: IRootState) => state.ehrLogin.accessCode;
export const getEhrLoginVerifyCodeSelector = (state: IRootState) => state.ehrLogin.verifyCode;
export const getEhrLoginExpireOnSelector = (state: IRootState) => state.ehrLogin.expireOn;
export const isEhrLoginActionLoadingSelector = (state: IRootState) => state.ehrLogin.isEhrActionLoading;
export const getEhrLoginSelectdIdSelector = (state: IRootState) => state.ehrLogin.selectedId;

