import React from 'react';
import { Route, Redirect } from "react-router-dom";
import { getToken  } from './helpers/auth';


const PublicRoute = ({...props}) => {

    return (
        <Route {...props}>
            {getToken('token') ? <Redirect to={'/'} /> :  <Redirect to={'/login'} />}
        </Route>
    )
}

export default PublicRoute;