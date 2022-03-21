import { IActionPayload, ELoginActions } from '../../actions';
import { IloginResponse, IloginRequest } from '../../actions/login';

export interface ILoginState {
   user: object | null;
   isLoading: boolean;
   token: string | null;
   email: string | null;
   password: string | null;
   refreshToken: string | null;
   expiresIn: number;
   errorList: Array<string>;
}

export const loginInitState: ILoginState = {
    user: null,
    isLoading: false,
    token: null,
    email: '',
    password: '', 
    refreshToken: null,
    expiresIn: 0,
    errorList: [],
}

const loginReducer = (state: ILoginState = loginInitState, action: IActionPayload) => {
    switch(action.type) {
        case ELoginActions.loginRequestStart:
            return {
                ...state,
                isLoading: true,
            }
        case ELoginActions.loginRequestError:
            return {
                ...state,
                isLoading: false,
            }
        case ELoginActions.loginRequestSuccess:
            return {
                ...state,
                isLoading: false,
                token: action.payload.AccessToken.Token,
                refreshToken: action.payload.RefreshToken,
                expiresIn: action.payload.AccessToken.ExpiresIn
            }
        case ELoginActions.setLoginErrorList:
            return {
                ...state,
                errorList: action.payload
            }
        default: return state;
    }
}

export default loginReducer;