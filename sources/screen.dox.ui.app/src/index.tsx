import React from 'react';
import ReactDOM from 'react-dom';
import { createGlobalStyle } from 'styled-components';
import App from './App';
import reportWebVitals from './reportWebVitals';
import { pxToRem } from './helpers/general';
import { createStore, applyMiddleware } from 'redux';
import createSagaMiddleware from 'redux-saga';
import { composeWithDevTools } from 'redux-devtools-extension';
import { Provider } from 'react-redux';
import reducers from './states';
import rootSaga from './sagas';


const sagaMiddleware = createSagaMiddleware();
const store = createStore(
  reducers,
  composeWithDevTools(applyMiddleware(sagaMiddleware))
);
sagaMiddleware.run(rootSaga);

const GlobalStyle = createGlobalStyle`
  html, body {
    font-family: hero-new, sans-serif !important;
    font-weight: 400;
    font-style: normal;
    height: 100%;
    overflow: hidden;
    background: #f4f6f8;
    font-size: 14px;
    line-height: 1.4;
  }
  main {
    font-family: hero-new, sans-serif !important;
    font-weight: 400;
    font-style: normal;
    background: #f4f6f8;
  }
  *, html, body {
      font-family: hero-new, sans-serif !important;
      font-weight: 400;
      font-style: normal;
      margin: 0;
      padding: 0;
      -webkit-text-size-adjust: 100%;
      font-family: 'hero-new', sans-serif;
  }

  ::-webkit-scrollbar {
    width: ${pxToRem(5)}; 
    height: ${pxToRem(5)}; 
  }

  ::-webkit-scrollbar-track {
    background: rgb(231, 231, 231);
    border-radius: ${pxToRem(1.5)};
  }

  ::-webkit-scrollbar-button {
    background: transparent;
    height: 0;
    pointer-event: none;
  }

  ::-webkit-scrollbar-thumb {
    background: rgb(158, 158, 158);
    border-radius: ${pxToRem(1.5)};
  }
  .MuiPopover-paper.My-Select-Menu {
    border: 1px solid gray;
    border-radius: 4px;
    box-shadow: 3px 3px 5px #2e2e42;
  }
  
`;

ReactDOM.render(
  <React.StrictMode>
    <GlobalStyle />
    <Provider store={store}>
      <App />
    </Provider>
  </React.StrictMode>,
  document.getElementById('root')
);

// If you want to start measuring performance in your app, pass a function
// to log results (for example: reportWebVitals(console.log))
// or send to an analytics endpoint. Learn more: https://bit.ly/CRA-vitals
reportWebVitals();
