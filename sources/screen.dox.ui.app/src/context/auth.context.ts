import React from 'react';

export interface IAuthContextState {
    isLoggedIn: boolean;
}

export const initialAuthContextState: IAuthContextState = {
    isLoggedIn: false,
} 

const AuthContext = React.createContext(initialAuthContextState);

export default AuthContext;

