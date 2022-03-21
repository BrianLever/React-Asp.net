import { IProfileResponse } from 'actions/profile';
import React, { FC } from 'react';
import { useSelector } from 'react-redux';
import { Route, Redirect } from "react-router-dom";
import { ERouterUrls } from 'router';
import { getProfileSelector } from 'selectors/profile';
import { getToken  } from './helpers/auth';
import { RouteProps } from 'react-router-dom';


interface PrivateRouteProps extends React.HTMLAttributes<Element>, RouteProps  {
    children: React.ReactNode;
    exact: boolean;
    key: string;
    path: string;
}

const PrivateRoute =  ({ children, ...rest }:PrivateRouteProps) => {
    const profile: IProfileResponse | null = useSelector(getProfileSelector);
    var path = ERouterUrls.LOGIN;
    
    if(getToken('token') && profile && profile.IsMustChangePassword) {
        path = ERouterUrls.CHANGE_PASSWORD;
    } else if(getToken('token') && profile && profile?.IsMustSetupSecurityQuestion) {
        path = ERouterUrls.CHANGE_SECURITY_QUESTION;
    }

    return (
        <Route {...rest}  render={(match) => {
            return getToken('token')
              ? children
              : <Redirect to={path}/>
          }} />
    )
 }


 export default PrivateRoute;