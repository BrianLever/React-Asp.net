import React, { Suspense } from 'react';
import { BrowserRouter as Router, Route, Switch, Redirect } from 'react-router-dom';
import ROUTES, {ERouterUrls, ICustomRouterProps} from './router';
import Loading from './components/UI/loading';
import Layout from './components/UI/layout';
import * as config from './config/app.json'
import { useDispatch, useSelector } from 'react-redux';
import { isGlobalLoading, getCurrentPageKeySelector } from './selectors/settings';
import { SnackbarProvider } from 'notistack';
import CustomNotification from './components/UI/notification';
import { getToken } from 'helpers/auth';
import PrivateRoute from './PrivateRoute';
import PublicRoute   from './PublicRoute';
import Login from 'components/pages/login';
import ResetPassword  from 'components/pages/reset-password';
import { refreshTokenRequest } from 'actions/login';

const App: React.FC = (): React.ReactElement => {
  
  const isLoading = useSelector(isGlobalLoading);
  const pageKey = useSelector(getCurrentPageKeySelector);
  const dispatch = useDispatch();

  React.useEffect(() => {
    document.title = `${pageKey} :: Screendox`;
  }, [pageKey]);

  return (
    <Router basename={config.BASEURL}>
      <Switch>
        {
          isLoading ? <Loading /> : null
        }
        <Route
           exact
           path={ERouterUrls.LOGIN}
        >
           <Login />
        </Route>
        <Route
           exact
           path={ERouterUrls.RESET_PASSWORD}
        >
           <ResetPassword />
        </Route>
        <SnackbarProvider 
          maxSnack={3}
          anchorOrigin={{
            vertical: 'top',
            horizontal: 'right',
          }}
        > 
          <CustomNotification />
          <Suspense fallback={<Loading />} >
            <Layout>
              {
                ROUTES.map((r: ICustomRouterProps) => {
                    const EComponent = r.component??'div';
                    const { component, ...other } = r;
                    if(r.public) {
                      return (
                        <Route 
                          exact
                          key={r.key}
                          path={r.path}>
                          <EComponent />
                        </Route>
                      )
                    } else {
                      return (
                        <PrivateRoute
                          exact
                          key={r.key}
                          path={r.path}
                        >
                          <EComponent {...r} />
                        </PrivateRoute>
                      )
                    }
                })
              }
            </Layout>
          </Suspense>
        </SnackbarProvider>
      </Switch>
    </Router>
  );
}

export default App;
