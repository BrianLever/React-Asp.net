import { IRootState } from '../../states';


export const loginEmailSelector = (state: IRootState) => state.login.email;
export const isLoginRequestLoadingSelector = (state: IRootState) => state.login.isLoading;
export const loginPasswordSelector = (state: IRootState) => state.login.password;
export const loginErrorListSelector = (state: IRootState) => state.login.errorList;