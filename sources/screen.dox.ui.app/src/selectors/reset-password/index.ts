import { IRootState } from '../../states';


export const resetPasswordSecurityQuestionSelector = (state: IRootState) => state.resetPassword.securityQuestion;
export const isResetPasswordSecurityQuestionLoadingSelector = (state: IRootState) => state.resetPassword.isSecurityQuestionLoading;
export const isResetPasswordSecurityQuestionLoadingErrorSelector = (state: IRootState) => state.resetPassword.isSecurityQuestionLoadingError;
export const isResetPasswordLoadingSelector = (state: IRootState) => state.resetPassword.isResetPasswordLoading;
export const isResetPasswordLoadingErrorSelector = (state: IRootState) => state.resetPassword.isResetPasswordLoadingError;
export const resetPasswordErrorsSelector = (state: IRootState) => state.resetPassword.resetPasswordErrors;
export const isResetPasswordSuccessSelector = (state: IRootState) => state.resetPassword.isResetPasswordSuccess;