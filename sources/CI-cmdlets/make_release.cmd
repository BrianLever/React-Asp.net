set /p assembly_version=< version.txt
set build_revision=%1


@echo Making a new release %assembly_version%, revision %build_revision%
@echo ===========================================================


@echo Setting Release version...
call setbuildversion.cmd %build_revision% 1

@echo ===========================================================
@echo Publishing Server API...
call publish_server_api.cmd


@echo ===========================================================
@echo Publishing Legacy Server...
call publish_server.cmd

@echo ===========================================================
@echo Publishing Services...
call publish_services.cmd

@echo ===========================================================
@echo Publishing Kiosk App...
call publish_kiosk_v2.cmd


@echo ===========================================================
@echo Publishing release packages...
call package_release.cmd


@echo ===========================================================
@echo COMPLETED
@echo ===========================================================