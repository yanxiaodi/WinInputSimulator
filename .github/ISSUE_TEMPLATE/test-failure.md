---
name: Test Failure Report
about: Report a failing test in the CI/CD pipeline or local environment
title: '[TEST FAILURE] '
labels: ['bug', 'test-failure']
assignees: ['yanxiaodi']
---

## Test Failure Details

**Test Name:** 
<!-- Name of the failing test method or test class -->

**Environment:** 
<!-- Check all that apply -->
- [ ] Local development environment
- [ ] GitHub Actions CI/CD
- [ ] Pull Request validation
- [ ] Scheduled/nightly tests

**Operating System:**
<!-- e.g., Windows 11, Windows 10, Windows Server 2022 -->

**Configuration:**
- [ ] Debug
- [ ] Release

## Failure Information

**Error Message:**
```
<!-- Paste the error message here -->
```

**Stack Trace:**
```
<!-- Paste the full stack trace here -->
```

**Test Output:**
```
<!-- Paste relevant test output here -->
```

## Expected vs Actual Behavior

**Expected:** 
<!-- What should have happened? -->

**Actual:** 
<!-- What actually happened? -->

## Reproduction Steps

1. 
2. 
3. 

## Additional Context

**Possible Causes:**
<!-- What might be causing this failure? -->
- [ ] DPI scaling issues
- [ ] Timing/synchronization issues
- [ ] Windows permissions
- [ ] UI session not available (headless)
- [ ] Environment-specific configuration
- [ ] Other: 

**System Information:**
- .NET Version: 
- Screen Resolution: 
- DPI Scaling: 
- Remote Desktop: Yes/No
- User Account Type: Administrator/Standard

## Related Links

- [ ] GitHub Actions workflow run: 
- [ ] Pull Request: 
- [ ] Related Issue: 

## Screenshots

<!-- If applicable, add screenshots to help explain the problem -->